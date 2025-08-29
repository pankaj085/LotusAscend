# LotusAscend - Database Schema

This document outlines the database schema for the LotusAscend - loyalty program. The database is designed to store member information, manage authentication, track points, and handle coupon redemptions.

### **Contents** - Tables in My Database
- Members
- OTPs
- PointTransactions
- Coupons

#### 1. Members

Stores the primary information for each registered user.


Schema:
```sql
| Column         | Data Type             | Constraints                     | Description                                            |
|----------------|-----------------------|---------------------------------|--------------------------------------------------------|
| Id           | integer             | Primary Key, Auto-increment   | The unique identifier for the member.                  |
| Username     | varchar(50)         | Unique, Not Null              | The member's chosen username.                          |
| MobileNumber | varchar(10)         | Unique, Not Null              | The member's 10-digit mobile number for login.         |
| IsVerified   | boolean             | Not Null, Default: false      | A flag indicating if the account is verified via OTP.  |
| TotalPoints  | integer             | Not Null, Default: 0          | The member's current spendable points balance.         |
| CreatedAt    | timestamp           | Not Null, Default: NOW()      | The date and time the member registered.               |
```

Sample Data:
```sql
| Id  | Username | MobileNumber | IsVerified | TotalPoints | CreatedAt                   |
|-----|----------|--------------|------------|-------------|-----------------------------|
| 1   | rogue  | 9876543210 | true     | 421750      | 2025-08-29 10:15:00       |
| 2   | mrLotus| 8508508508 | true     | 0           | 2025-08-29 11:30:00       |
```
#### 2. OTPs

A temporary table to store One-Time Passwords for verification.

Schema:
```sql
| Column     | Data Type | Constraints                | Description                                        |
|------------|-----------|----------------------------|----------------------------------------------------|
| Id         | integer   | Primary Key, Auto-increment| The unique identifier for the OTP record.          |
| MemberId   | integer   | Foreign Key (Members.Id)   | Links the OTP to a specific member.                |
| Code       | varchar(4)| Not Null                   | The 4-digit verification code.                     |
| Expiry     | timestamp | Not Null                   | The date and time when the OTP will become invalid.|
```

Sample Data (before verification):
```sql
| Id  | MemberId | Code   | Expiry                |
|-----|----------|--------|-----------------------|
| 101 | 3        | 2708 | 2025-08-29 16:39:00 |
```

#### 3. PointTransactions

Logs every transaction where a member earns points.
```sql
Schema:
| Column           | Data Type | Constraints                | Description                                  |
|------------------|-----------|----------------------------|----------------------------------------------|
| Id             | integer   | Primary Key, Auto-increment | The unique identifier for the transaction.    |
| MemberId       | integer   | Foreign Key (Members.Id)    | Links the transaction to a member.            |
| PurchaseAmount | numeric   | Not Null                    | The purchase amount that generated the points.|
| PointsAdded    | integer   | Not Null                    | The number of points awarded.                 |
| TransactionDate| timestamp | Not Null, Default: NOW()    | The date and time of the transaction.         |
```

Sample Data:
```sql
| Id  | MemberId | PurchaseAmount | PointsAdded | TransactionDate       |
|-----|----------|----------------|-------------|-----------------------|
| 501 | 1        | 500.00         | 50          | 2025-08-29 12:05:00 |
| 502 | 1        | 1250.00        | 120         | 2025-08-29 14:20:00 |
```
#### 4. Coupons

Stores a record of every coupon redeemed by a member.

Schema:
```sql
| Column         | Data Type      | Constraints                | Description                                       |
|----------------|----------------|----------------------------|---------------------------------------------------|
| Id             | integer      | Primary Key, Auto-increment| The unique identifier for the coupon.             |
| MemberId       | integer      | Foreign Key (Members.Id)   | Links the coupon to the member who redeemed it.   |
| PointsRedeemed | integer      | Not Null                   | The number of points spent to get the coupon.     |
| CouponValue    | numeric      | Not Null                   | The monetary value of the discount (e.g., in ₹).  |
| CouponCode     | varchar(8)   | Not Null                   | The unique, generated 8-character code.           |
| RedemptionDate | timestamp    | Not Null, Default: NOW()   | The date and time the coupon was redeemed.        |
```
Sample Data:
```sql
| Id  | MemberId | PointsRedeemed | CouponValue | CouponCode | RedemptionDate        |
|-----|----------|----------------|-------------|------------|-----------------------|
| 21  | 1        | 1000           | 100.00      | YFEUSNE1 | 2025-08-29 16:10:00 |
| 22  | 2        | 500            | 50.00       | K2P9XG4H | 2025-08-29 16:12:00 |
```