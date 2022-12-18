#include <iostream>
#include <thread>
#include <ctime>

using namespace std;

void do1()
{
	for (int i = 0; i < 10; i++)
	{
		this_thread::sleep_for(chrono::milliseconds(500));
		cout << "Working of " << this_thread::get_id() << " thread//do1" << endl;
	}
}

void do2()
{
	for (int i = 0; i < 10; i++)
	{
		this_thread::sleep_for(chrono::milliseconds(1000));
		cout << "Working of " << this_thread::get_id() << " thread//do2" << endl;
	}
}

void do3()
{
	for (int i = 0; i < 10; i++)
	{
		this_thread::sleep_for(chrono::milliseconds(2000));
		cout << "Working of " << this_thread::get_id() << " thread//do3" << endl;
	}
}

int main()
{
	double start_time = clock();

	thread do11(do1);
	thread do22(do2);
	thread do33(do3);

	do11.join();
	do22.join();
	do33.join();
	double end_time = clock();
	double search_time = end_time - start_time;
	cout << "time:"<<search_time/1000;
}