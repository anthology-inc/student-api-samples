#!/bin/env python
from __future__ import print_function
import requests
"""
A sample Python program demonstrating usage of the CampusNexus OData Query API
See https://github.com/campusmanagement/integration-samples/
and http://community.campusmgmt.com/1329-2/
"""

if __name__ == '__main__':
    # Set the following variables to match your environment.
    root_uri = "http://university-a.campusnexus.cloud/" # Web Client URI
    username = "user@university-a.campusnexus.cloud"
    password = "password"

    # Create a persistent session in order to maintain authentication
    with requests.Session() as s:
        # set authentication header
        s.auth = (username, password)

        # Get the list of courses
        courses_uri = "{0}ds/odata/Courses?$select=Code,Name".format(root_uri)
        r = s.get(courses_uri)
        r.raise_for_status()

        result = r.json()

        for child in result.get("value"):
            print(child["Code"])
            print(child["Name"])
