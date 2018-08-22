#!/bin/env python
from __future__ import print_function
import requests
from functools import reduce
"""
A sample Python program demonstrating usage of the CampusNexus Command Web API
See https://github.com/campusmanagement/integration-samples/
"""

class CommandModelClient(object):
    """A convenience wrapper for Command API calls"""
    def __init__(self, client, entity_url):
        self._client = client
        self._entity_url = entity_url.rstrip('/')

    def execute_command(self, command, value={}):
        data = {"payload": value}
        command_url = "{0}/{1}".format(self._entity_url, command)
        r = self._client.post(command_url, json=data)
        r.raise_for_status()
        return r.json()

    def create(self):
        return self.execute_command("create")

    def delete(self, entity):
        return self.execute_command("delete", entity)

    def save_new(self, entity):
        return self.execute_command("saveNew", entity)

    def save(self, entity):
        return self.execute_command("save", entity)

    def retrieve(self, id_value):
        body = {"id": id_value}
        return self.execute_command("get", body)


def select_token(dictionary, keys, default=None):
    """Utility function to streamline retrieval from deeply nested dicts
    keys should be dot-separated
    default is returned if not found
    """
    return reduce(lambda d, key: d.get(key, default) if isinstance(d, dict) else default, keys.split("."), dictionary)


if __name__ == "__main__":
    # Set the following variables to match your environment.
    # Username needs authorization in CampusNexus Web Client Security Console
    # to "Academics - Configuration - Manage" task
    root_uri = "http://university-a.campusnexus.cloud/" # Web Client URI
    username = "user@university-a.campusnexus.cloud"
    password = "password"

    with requests.Session() as s:
        s.auth = (username, password)
        command_uri = "{0}/api/commands/Academics/CourseType".format(root_uri)

        command_model_client = CommandModelClient(s, command_uri)

        # create
        print("Creating...")
        result = command_model_client.create()

        # save new
        print("Saving...")
        data = select_token(result, "payload.data")
        data["code"] = "Test"
        data["name"] = "Test"
        result = command_model_client.save_new(data)

        ## update
        print("Updating...")
        data = select_token(result, "payload.data")
        data["name"] = "Test2"
        result = command_model_client.save(data)

        ## retrieve
        print("Retrieving...")
        id_token = select_token(result, "payload.data.id")
        result = command_model_client.retrieve(id_token)

        # delete
        print("Deleting...")
        data = select_token(result, "payload.data")
        result = command_model_client.delete(data)

