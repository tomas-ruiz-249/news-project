import httpx
import json


class ApiService:
    def __init__(self, api_url) -> None:
        self.api_url = api_url
        self.client = httpx.Client()

    def get_articles(self):
        response = self.client.get(self.api_url + "/articles")
        if response.status_code == 200:
            articles = json.loads(response.text)
            return articles
        else:
            raise Exception("equisde")
