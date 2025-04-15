Feature: OrderManagerTest

    Background:
        Given Now is 2023-10-01
        Given Throwing an exception
        And Products as below
          | Name     | Cost |
          | product1 | 20   |
          | product2 | 12   |
        And Assets as below
          | UserName | Value |
          | user1    | 50    |
          | user2    | 80    |
        And Start harness

    Scenario: Create order
        When Create order as below
          | UserName | ProductName | Quantity |
          | user1    | product1    | 2        |
        Then Order as below
          | UserName | Amount | Status  |
          | user1    | 40     | Created |

    Scenario: Order pending
        When Create order as below
          | UserName | ProductName | Quantity |
          | user1    | product1    | 2        |
        When Create payment as below
          | UserName |
          | user1    |
        Then Order as below
          | UserName | Amount | Status  |
          | user1    | 40     | Pending |

    Scenario: Order paid
        When Create order as below
          | UserName | ProductName | Quantity |
          | user1    | product1    | 2        |
        When Create payment as below
          | UserName |
          | user1    |
        When Run Consumer
        Then Asset as below
          | UserName | Value |
          | user1    | 10    |
        When Run Consumer
        Then Order as below
          | UserName | Amount | Status |
          | user1    | 40     | Paid   |

    Scenario: Insufficient balance
        Given Not throwing an exception
        When Create order as below
          | UserName | ProductName | Quantity |
          | user1    | product1    | 3        |
        When Create payment as below
          | UserName |
          | user1    |
        Then Exception is "InsufficientBalance"