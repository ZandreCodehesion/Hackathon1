#include <iostream>
#include <string>
#include <chrono>
#include <ctime>
#include <thread>

#include <curl/curl.h>

#include "json.hpp"

using json = nlohmann::json;
using namespace std::chrono;
using namespace std;

size_t callback(const char* in, size_t size, size_t num, std::string* out) {
    const size_t totalBytes(size * num);
    out->append(in, totalBytes);
    return totalBytes;
}

class  HTTP
{
  public:
    bool makeReq(string payload)
    {
      const string url("https://hackathon-validator.vercel.app/api/verify");
      const string json_payload(payload);  // You can replace it with your own JSON payload

      CURL* curl = curl_easy_init();

      // Set remote URL.
      curl_easy_setopt(curl, CURLOPT_URL, url.c_str());

      // Set POST data.
      curl_easy_setopt(curl, CURLOPT_POSTFIELDS, json_payload.c_str());

      // Verbose debugging.
      // curl_easy_setopt(curl, CURLOPT_VERBOSE, 1L);

      // Response information.
      long httpCode(0);
      unique_ptr<string> httpData(new string());

      // Hook up data handling function.
      curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, callback);

      // Hook up data container (will be passed as the last parameter to the
      // callback handling function).  Can be any pointer type, since it will
      // internally be passed as a void pointer.
      curl_easy_setopt(curl, CURLOPT_WRITEDATA, httpData.get());

      // Run our HTTP POST command, capture the HTTP response code, and clean up.
      curl_easy_perform(curl);
      curl_easy_getinfo(curl, CURLINFO_RESPONSE_CODE, &httpCode);
      curl_easy_cleanup(curl);


      if (httpCode == 200) {
        // Response looks good - done using Curl now.  Try to parse the results
        // and print them out.
        json jsonData = json::parse(*httpData);
        bool success = jsonData["success"];
        string message = jsonData["message"];

        return success;
      }
      else {
        cout << "Couldn't GET from " << url << " - exiting" << endl;
        return false;
      }
    }
};

class Worker
{
  public:
    string Benchmark() {
        HTTP http;
        if (http.makeReq(the_work())) {
            auto timerStart = steady_clock::now();
            int count = 0;
            string retro = "";
            bool stop = false;
            
            // Start multiple threads
            const int numThreads = std::thread::hardware_concurrency();
            std::vector<std::thread> threads;
            threads.reserve(numThreads);
            
            for (int i = 0; i < numThreads; ++i) {
                threads.emplace_back([this, &timerStart, &count, &stop] {
                    while (!stop) {
                        the_work();
                        count++;
                        if (duration_cast<seconds>(steady_clock::now() - timerStart).count() >= 5) {
                            stop = true;
                            break;
                        }
                    }
                });
            }
            
            // Wait for all threads to finish
            for (auto& thread : threads) {
                thread.join();
            }
            
            float result = duration_cast<microseconds>(steady_clock::now() - timerStart).count() / 1000000.0;
            retro += to_string(count) + ";" + to_string(result) + ";";
            
            return retro;
        }
        else {
            return "Error";
        }
    }

    string the_work()
    {
      if(true)
      {
        return "[1,2,3,4,5]";
      }
      else
      {
        return "";
      }
    }
};

int main()
{
  Worker worker;
  cout<< worker.Benchmark() << endl;
  return 0;
}
