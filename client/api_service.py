from asyncio import sleep
import httpx
import json


class ApiService:
    def __init__(self, api_url) -> None:
        self.api_url = api_url

    async def get_articles(self):
        async with httpx.AsyncClient() as client:
            response = await client.get(self.api_url + "/articles")
            if response.status_code == 200:
                articles = json.loads(response.text)
                return articles
            else:
                raise Exception("HTTP ERROR")
