from api_service import ApiService
from textual.app import App, ComposeResult
from textual.widgets import (
    Collapsible,
    Footer,
    Header,
    LoadingIndicator,
    Static,
    TabPane,
    TabbedContent,
)


class Client(App):
    TITLE = "News Client"
    SUB_TITLE = "all your news in one place"

    api = ApiService("http://localhost:5039/api")

    def compose(self) -> ComposeResult:
        yield Header()
        yield LoadingIndicator()
        yield TabbedContent()
        yield Footer()

    async def on_mount(self) -> None:
        await self.fetch_content()
        self.query_one(LoadingIndicator).remove()

    async def fetch_content(self):
        articles = self.api.get_articles()
        sites = set([a["siteName"] for a in articles])
        tabbed = self.query_one(TabbedContent)
        for s in sites:
            site_articles = [a for a in articles if a["siteName"] == s]
            collapsibles = [
                Collapsible(
                    Static(
                        a["author"] + " " + str(a["publicationDate"]) + " " + a["body"]
                    ),
                    title=a["title"],
                )
                for a in site_articles
            ]
            await tabbed.add_pane(TabPane(s, *collapsibles))

    def on_button_pressed(self) -> None:
        self.exit()
