#include <iostream>
#include <string>

#include <curl/curl.h>

#include "json.hpp"

using namespace std;
using json = nlohmann::json;

class HTTP {
  public:
  void makeReq(const string& payload) {
    const string url("https://hackathon-validator.vercel.app/api/verify");
    string response;

    CURL* curl = curl_easy_init();
    if (curl) {
      // Set the URL
      curl_easy_setopt(curl, CURLOPT_URL, url.c_str());

      // Set the request type to POST
      curl_easy_setopt(curl, CURLOPT_POST, 1L);

      // Set the request payload (JSON data)
      curl_easy_setopt(curl, CURLOPT_POSTFIELDS, payload.c_str());

      // Response callback function
      curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, WriteCallback);
      curl_easy_setopt(curl, CURLOPT_WRITEDATA, &response);

      // Perform the request
      CURLcode res = curl_easy_perform(curl);

      // Check for errors
      if (res != CURLE_OK) {
        cerr << "curl_easy_perform() failed: " << curl_easy_strerror(res) << endl;
      } else {
        // Parse the JSON response
        json jsonResponse = json::parse(response);

        // Access the response values
        bool success = jsonResponse["success"];
        string message = jsonResponse["message"];

        // Print the response values
        cout << "Success: " << boolalpha << success << endl;
        cout << "Message: " << message << endl;
      }

      // Cleanup
      curl_easy_cleanup(curl);
    }
  }
};

int main() {
  HTTP http;
  http.makeReq("test");
  return 0;
}