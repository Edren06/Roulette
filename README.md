# SQL Script (Question 1)
The SQL script for question 1 has been added to the root of this project called pr_GetOrderSummary

# Roulette (Question 2)
This application has been created with Entity Framework and connects to a sql lite database. I have used dependency inject to inject the data context and the roulette service so that it is accessable to the application. Since there are finite options I have mapped the payout ratios in the SQL LITE database. There is also a UNIT Testing project that you can use to check the tests.

The application has been built in .net 6 and swagger can be used to test the app, the API calls are as follows:

PlaceBet (Accepts an amount and the name of the location you would want to bet) - various names are as follows and accepted as strings with their payout ratios, I have unfortunately omitted the doubles
and square bets due to time constraints:

1 - (35:1)

2 - (35:1)

3 - (35:1)

4 - (35:1)

5 - (35:1)

6 - (35:1)

7 - (35:1)

8 - (35:1)

9 - (35:1)

10 - (35:1)

11 - (35:1)

12 - (35:1)

13 - (35:1)

14 - (35:1)

15 - (35:1)

16 - (35:1)

17 - (35:1)

18 - (35:1)

19 - (35:1)

20 - (35:1)

21 - (35:1)

22 - (35:1)

23 - (35:1)

24 - (35:1)

25 - (35:1)

26 - (35:1)

27 - (35:1)

28 - (35:1)

29 - (35:1)

30 - (35:1)

31 - (35:1)

32 - (35:1)

33 - (35:1)

34 - (35:1)

35 - (35:1)

36 - (35:1)

0 - (35:1)

Red - (1:1)

Black - (1:1)

1-18 - (1:1)

19-36 - (1:1)

Odd - (1:1)

Even - (1:1)

FirstRow - (2:1)

SecondRow - (2:1)

ThirdRow - (2:1)

FirstColumn - (2:1)

SecondColumn - (2:1)

ThirdColumn - (2:1)

