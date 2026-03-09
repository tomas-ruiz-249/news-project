from api_service import ApiService
from textual.app import App, ComposeResult
from textual.widgets import (
    Collapsible,
    Footer,
    Header,
    TabPane,
    TabbedContent,
    Markdown,
)
from datetime import datetime

import locale

locale.setlocale(locale.LC_TIME, "es_CO.UTF-8")


class Client(App):
    TITLE = "News Client"
    SUB_TITLE = "all your news in one place"

    api = ApiService("http://localhost:5039/api")

    def compose(self) -> ComposeResult:
        yield Header()
        yield TabbedContent()
        yield Footer()

    def on_mount(self) -> None:
        tabbed = self.query_one(TabbedContent)
        tabbed.loading = True
        self.run_worker(self.load_data(tabbed), exclusive=True)

    async def load_data(self, tabbed) -> None:
        self.articles = await self.api.get_articles()
        self.sites = set([a["siteName"] for a in self.articles])

        for s in self.sites:
            site_articles = [a for a in self.articles if a["siteName"] == s]
            collapsibles = []
            for a in site_articles:
                collapsibles.append(
                    Collapsible(
                        title=a["title"],
                        id="article" + a["id"],
                    )
                )

            tabbed.add_pane(TabPane(s, *collapsibles))
        tabbed.loading = False

    async def on_collapsible_expanded(self, event: Collapsible.Toggled):
        if event.collapsible.collapsed:
            return
        if event.collapsible.query(Markdown):
            return
        id = str(event.collapsible.id).replace("article", "")
        articles = [a for a in self.articles if a["id"] == id]
        for a in articles:
            markdown_widget = Markdown(
                f"# {a["title"]}\n"
                + f"# [Ver original]({a["url"]})\n\n"
                + f"## Por: {a["author"]}\n"
                + f"## Publicado en: {
                    datetime.fromisoformat(a["publicationDate"]).strftime("%d %B %Y, %I:%M %p")
                    }\n"
                + f"## Extraido en: {
                    datetime.fromisoformat(a["extractionDate"]).strftime("%d %B %Y, %I:%M %p")
                }\n"
                + f"<p>{a["body"]}<p>"
            )
            await event.collapsible.query_one("Contents").mount(markdown_widget)

    def on_button_pressed(self) -> None:
        self.exit()
