#!/usr/bin/env ruby
# A sample Ruby program demonstrating usage of the CampusNexus OData Query API
# See https://github.com/campusmanagement/integration-samples/
# and http://community.campusmgmt.com/1329-2/

require 'faraday'
require 'faraday_middleware'

# set the folliwng variables to match your environment.
root_uri = 'http://university-a.campusnexus.cloud/' # Web Client URI
username = 'user@university-a.campusnexus.cloud'
password = 'password'

odata_prefix = 'ds/odata/'
url = "#{root_uri}#{odata_prefix}"

# prepare the connection, set authentication header
conn = Faraday.new(url: url) do |faraday|
  faraday.basic_auth(username, password)
  faraday.response :json
  faraday.adapter Faraday.default_adapter
end

# Get the first 10 Courses that have names starting with A.
filter_by = {'$select': 'Code,Name', '$top': '10',
  '$filter': "startswith(Name, 'A') eq true",
  '$orderby': 'Name'}
response = conn.get('Courses', filter_by)
result = response.body
# Debug
#puts result

result['value'].each do |child|
  puts child['Code']
  puts child['Name']
end
