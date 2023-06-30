#include <cstdio>
#include <sstream>
#include <chrono>
#include <ctime>
#include <cstdlib>
#include <vector>
#include <list>
#include <cmath>
#include <map>
#include <bitset>

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
        const std::string json_payload(payload);

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
            return true;
        }
        else {
            return false;
        }

        return false;
    }
};

class TesterWorker
{
    public:
        int primes_size = 1000000;
        // Duplicate Code, but this one returns the result
        string run_worker_once()
        {
            std::bitset<1000000> bit_array;
            bit_array.set().all();

            // Always non-prime, no need to check
            bit_array.reset(0); // 0
            bit_array.reset(1); // 1

            int counter = 0;

            for(int i = 2; i * i < primes_size; i++)
            {
                if(bit_array.test(i))
                {
                    counter = i*i;
                    while(counter < primes_size)
                    {
                        bit_array.reset(counter);
                        counter += i;
                    }
                }    
            }

            list<int> ar;
            string s;
            stringstream o;
            
            int num = 0;
            for(int i = 0; i < 1000000; i++)
            {
                if(bit_array.test(i))
                {
                    ar.push_back(i);
                }
            }

            o << "[";

            for(int x : ar)
            {
                o << x;

                if(x == ar.back())
                    break;    

                o << ',';
            }

            o << "]";
            s = o.str();
            return s;
        }
};

class Worker
{
    public:
        int primes_size = 1000001;
        
        // This is the one doing the work
        void do_some_work()
        {
            // 1 Bit for each number, true for prime, false for non-prime 
            std::bitset<1000001> bit_array;
            bit_array.set().all();

            // Always non-prime, no need to check
            bit_array.reset(0); // 0
            bit_array.reset(1); // 1

            int counter = 0;

            for(int i = 1; i * i < primes_size; i++)
            {
                if(bit_array.test(i))
                {
                    counter = i*i;
                    while(counter < 1000001)
                    {
                        bit_array.reset(counter);
                        counter += i;
                    }
                }    
            }

            int num = 0;
            for(int i = 0; i < 1000001; i++)
            {
                if(bit_array.test(i))
                {
                    num+= 1;
                }
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
    string request = tester.run_worker_once();
    return(http.makeReq(request));
}


int main() {

    // Runs a single pass and validate the results
    if(!validator())
    {
        exit(1);
    }

    // Init worker class
    auto worker = Worker();
    int count = 0;
    auto tStart = steady_clock::now();
    // End init

    //worker.do_some_work();
    
    while(true)
    {
        // Put the code to be tested below
        worker.do_some_work();
        // End of tested code

        count++;

        if(duration_cast<seconds>(steady_clock::now() - tStart).count() >= 5)
        {
            float result = duration_cast<microseconds>(steady_clock::now() - tStart).count() / 1000000.0;
            string out;
            stringstream s;
            s << count << ";" << result << ";";
            out = s.str();
            printf("%s",out.c_str());
            break;
        }
    }
    return 0;
}