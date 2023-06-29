#include <iostream>
#include <sstream>
#include <chrono>
#include <ctime>
#include <cstdlib>
#include <vector>
#include <list>
#include <cmath>
#include <map>

#include <curl/curl.h>
#include <iterator>
#include <algorithm>

#include "json.hpp"

using json = nlohmann::json;

using namespace std;
using namespace std::chrono;

size_t callback(const char* in, size_t size, size_t num, std::string* out) {
    const size_t totalBytes(size * num);
    out->append(in, totalBytes);
    return totalBytes;
}

class Http 
{
    public:
    bool makeReq(string payload)
    {
        const std::string url("https://hackathon-validator.vercel.app/api/verify");
        const std::string json_payload(payload);  // You can replace it with your own JSON payload

        CURL* curl = curl_easy_init();

        // Set remote URL.
        curl_easy_setopt(curl, CURLOPT_URL, url.c_str());

        // Set POST data.
        curl_easy_setopt(curl, CURLOPT_POSTFIELDS, json_payload.c_str());

        // Verbose debugging.
        // curl_easy_setopt(curl, CURLOPT_VERBOSE, 1L);

        // Response information.
        long httpCode(0);
        std::unique_ptr<std::string> httpData(new std::string());

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
            //std::cout << "\nGot successful response from " << url << std::endl;

            // Response looks good - done using Curl now.  Try to parse the results
            // and print them out.
            json jsonData = json::parse(*httpData);

            bool success = jsonData["success"];

            if(success)
                return true;
            
            if(!success)
                return false;
        }
        else {
            return false;
        }

        return false;
    }
};

class TesterWorker
{
    private:
        int counter = 0;

        // Duplicate Code, but this one returns the result
        int run_worker_once()
        {
            return counter++;
        }

    public:

        // Formats the results for API req
        string worker_for_validator()
        {
            list<int> ar;
            string s;
            stringstream o;

            for(int i =0; i > 6; i++)
                ar.push_back(this->run_worker_once());
            
            o << "\"[";

            for(int x : ar)
            {
                o << x;

                if(x == ar.back())
                    break;    

                o << ',';
            }

            o << "]\"";
            s = o.str();

            return s;
        }
};

class Worker
{
    public:
        int random = 0;
        
        void do_some_work()
        {
            random++;
        }

        void unit_vector()
        {
            int x = rand() % (1000);
            int y = rand() % (1000);
            int z = rand() % (1000);

            double length = sqrt(((x * x) + (y * y) + (z * z)));

            double x_l = x / length; 
            double y_l = y / length; 
            double z_l = z / length;
            
            double unit_length = sqrt(((x_l * x_l) + (y_l * y_l) + (z_l * z_l)));

            if (((unit_length - 1)*(unit_length - 1)) > 0.003)
            {
                exit(1);
            }
        }
};

bool validator()
{   
    // This fires up a tester class that runs the same
    // sieve as worker but with a return value added
    auto tester = TesterWorker();
    auto http = Http();
    // Runs the tester worker and compiles the array for single validation
    string request_array = tester.worker_for_validator();
    return(http.makeReq(request_array));
}


int main() {

    // Bit arrays
    bitset<10> bit_array;
    bit_array.set().all();

    // String to charr array
    string test = "thisIsSomRandomTestString";
    char* char_arr = test.data();
    //cout << char_arr[5] << endl;

    // Runs a single pass and validate the results
    //if(!validator)
    //{
    //    exit(1);
    //}

    // Init worker class
    auto worker = Worker();
    int count = 0;
    auto tStart = steady_clock::now();
    // End init

    while(true)
    {
        // Put the code to be tested below
        worker.do_some_work();
        // End of tested code

        count++;

        if(duration_cast<seconds>(steady_clock::now() - tStart).count() >= 5)
        {
            float result = duration_cast<microseconds>(steady_clock::now() - tStart).count() / 1000000.0;
            cout << count << ";" << result << ";" << endl;
            break;
        }
    }
    return 0;
}