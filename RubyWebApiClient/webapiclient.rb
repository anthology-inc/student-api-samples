require 'faraday'
require 'faraday_middleware'
require 'ruby_dig'

# A sample Ruby program demonstrating usage of the CampusNexus Command Web API
# See https://github.com/campusmanagement/integration-samples/

class CommandModelClient
  attr_accessor :client
  attr_accessor :entity_url

  def initialize(client, entity_url)
    @client = client
    @entity_url = entity_url.strip.sub(/\/$/, '')
  end

  def execute_command(command, value=nil)
    command_url = "#{entity_url}/#{command}"
    r = client.post(command_url, { "payload" => value })
    r.body
  end

  def create
    execute_command('create')
  end

  def delete(entity)
    execute_command('delete', entity)
  end

  def save_new(entity)
    execute_command('saveNew', entity)
  end

  def save(entity)
    execute_command('save', entity)
  end

  def retrieve(id_value)
    body = { "id" => id_value }
    execute_command('get', body)
  end
end


def main
  # Set the following variables to match your environment.
  # Username needs authorization in CampusNexus Web Client Security Console
  # to "Academics - Configuration - Manage" task
  root_uri = 'http://university-a.campusnexus.cloud/' # Web Client URI
  username = 'user@university-a.campusnexus.cloud'
  password = 'password'

  url = "#{root_uri}/api/commands/"

  conn = Faraday.new(url: url) do |faraday|
    faraday.basic_auth(username, password)
    faraday.request  :json # Encode the request as JSON
    faraday.response :json # Decode the response from JSON
    #faraday.response :logger # Output to log
    faraday.adapter Faraday.default_adapter
  end

  command_uri = "Academics/CourseType"
  command_model_client = CommandModelClient.new(conn, command_uri)

  # create
  puts "Creating..."
  result = command_model_client.create

  # save new
  puts "Saving..."
  data = result.dig('payload','data')
  data["code"] = "Test"
  data["name"] = "Test"
  result = command_model_client.save_new(data)

  # update
  puts "Updating..."
  data = result.dig('payload','data')
  data["name"] = "Test2"
  result = command_model_client.save(data)

  # retrieve
  puts "Retrieving..."
  id_token = result.dig('payload','data', 'id')
  result = command_model_client.retrieve(id_token)

  # delete
  puts "Deleting..."
  data = result.dig('payload','data')
  result = command_model_client.delete(data)
end

# Running as a program
if __FILE__ == $0
  main
end
