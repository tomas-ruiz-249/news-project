from textual.app import App, ComposeResult
from textual.widgets import Collapsible, Header, Label

from api_service import ApiService


class Client(App):
    TITLE = "News Client"
    SUB_TITLE = "all your news in one place"

    api = ApiService("http://localhost:5039/api")

    def compose(self) -> ComposeResult:
        yield Header()
        articles = self.api.get_articles()
        for a in articles:
            with Collapsible(title=a["title"]):
                yield Label(a["body"])

    def on_button_pressed(self) -> None:
        self.exit()
