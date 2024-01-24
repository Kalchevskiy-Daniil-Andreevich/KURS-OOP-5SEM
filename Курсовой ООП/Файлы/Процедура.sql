CREATE OR ALTER PROCEDURE checkCredential
    @username NVARCHAR(50),
    @password NVARCHAR(50),
    @isValid BIT OUTPUT,
  @userType NVARCHAR(50) OUTPUT
AS
BEGIN
    SET @isValid = 0;
    IF EXISTS (
    SELECT * 
    FROM LOGPAS 
    WHERE LOGIN_VALUE = @username 
    AND PASSWORD_VALUE = @password)
    BEGIN
        SET @isValid = 1;
    SET @userType = (
    SELECT USER_TYPE 
    FROM LOGPAS 
    WHERE login_value = @username
    )
    END
END

INSERT INTO LOGPAS (login_value, password_value, user_type)
VALUES ('client@gmail.com', 'client', 'client');
INSERT INTO LOGPAS (login_value, password_value, user_type)
VALUES ('admin@gmail.com', 'admin', 'admin');
INSERT INTO LOGPAS (login_value, password_value, user_type)
VALUES ('client2@gmail.com', 'client', 'client');
INSERT INTO LOGPAS (login_value, password_value, user_type)
VALUES ('client3@gmail.com', 'client', 'client');

Select * from LOGPAS;
DELETE FROM LOGPAS;

go
CREATE OR ALTER PROCEDURE RegisterClient
    @lastName VARCHAR(50),
    @firstName VARCHAR(50),
    @middleName VARCHAR(50),
    @age INT,
    @gender VARCHAR(10),
    @phone VARCHAR(20),
	@username VARCHAR(50),
	@password_value VARCHAR(50),
  @isValid BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
  SET @isValid = 0;
    DECLARE @id_authentification INT;
    IF NOT EXISTS (SELECT 1 FROM CLIENTS WHERE PHONENAME_CLIENT = @phone)
	BEGIN
	IF NOT EXISTS (SELECT 1 FROM LOGPAS WHERE LOGIN_VALUE = @username)
	 BEGIN
        INSERT INTO LOGPAS (login_value, password_value, user_type)
        VALUES (@username, @password_value, 'CLIENT');
        SET @id_authentification = SCOPE_IDENTITY();
        INSERT INTO CLIENTS (LASTNAME_CLIENT, FIRSTNAME_CLIENT, MIDDLENAME_CLIENT, AGE_CLIENT, GENDER_CLIENT, PHONENAME_CLIENT, ID_LOGPAS)
        VALUES (@lastName, @firstName, @middleName, @age, @gender, @phone, @id_authentification);
    SET @isValid = 1;
    END
	END
END;

GO
DECLARE
	@isValid BIT;
BEGIN
	EXEC RegisterClient 'v', 'v', 'v', 4, 'v' , 'v', 'v', 'v' , @isValid;
END

INSERT INTO CLIENTS(ID_LOGPAS, FIRSTNAME_CLIENT, LASTNAME_CLIENT, MIDDLENAME_CLIENT, AGE_CLIENT, GENDER_CLIENT, PHONENAME_CLIENT)
VALUES(8, '8', '8', '8', 8, '8', '8');

SELECT * FROM CLIENTS;
DELETE FROM CLIENTS;

SELECT * FROM LOGPAS;


SELECT * FROM ABONEMENTS;
SELECT * FROM ABONEMENT_REQUEST;
SELECT * FROM SALEOFABONEMENTS;

Delete FROM  SALEOFABONEMENTS;
Select * from EMPLOYEES;

INSERT INTO ABONEMENT_REQUEST VALUES (12, 90, 'SDGDF', 'FDGFD', '01-01-2021');
INSERT INTO SALEOFABONEMENTS VALUES (12, 90, '02-02-2021', '01-01-2021');

INSERT INTO ABONEMENTS (PRICE_ABONEMENT, AMOUNT_OF_VISITS, IMG_ABONEMENT, TYPE_OF_ABONEMENT, TYPE_OF_EXERCISE, ADDITIONAL_SERVICE)
VALUES
    (100.00, 10, 0x012345, '��������������', '����������� ���', '�����'),
    (150.00, 20, 0x6789AB, '��������', '��������� �������', '������'),
    (200.00, 30, 0xCDEF01, '�������������', '�������', '�����');

Delete From ABONEMENTS;


INSERT INTO CLIENTS(ID_LOGPAS, FIRSTNAME_CLIENT, LASTNAME_CLIENT, MIDDLENAME_CLIENT, AGE_CLIENT, GENDER_CLIENT, PHONENAME_CLIENT)
VALUES(18, '������', '�����������', '���������', 19, '�������', '375445403394');

SELECT * FROM EMPLOYEES;

INSERT INTO EMPLOYEES (ID_LOGPAS, FIRSTNAME_EMPLOYEE, LASTNAME_EMPLOYEE, PHONENAME_EMPLOYEE, SALARY_EMPLOYEE)
VALUES
	(17, 'Joh', 'Do', '555-1234', 30000),
    (18, 'John', 'Doe', '555-1234', 50000),
    (19, 'Jane', 'Smith', '555-5678', 60000),
    (20, 'Bob', 'Johnson', '555-9876', 55000);

INSERT INTO ABONEMENT_REQUEST (ID_CLIENT, ID_ABONEMENT, TYPE_OF_TRAINING, TYPE_OF_SERVICE, DATA_REQUEST)
VALUES
  (10, 91, 'Cardio', 'Regular', '2023-01-15'),
  (12, 92, 'Strength', 'Premium', '2023-02-20');


Delete From ABONEMENTS Where ID_ABONEMENT = 93;

INSERT INTO ABONEMENTS (PRICE_ABONEMENT, AMOUNT_OF_VISITS, IMG_ABONEMENT, TYPE_OF_ABONEMENT, TYPE_OF_EXERCISE, ADDITIONAL_SERVICE)
VALUES
    (100.00, 31, 0x01, '�������', '������', '������ � ����������'),
    (150.00, 30, 0x02, '��������', '������� ��������', '������ � ��������'),
    (200.00, 60, 0x03, '�������', '����', '������ � �����'),
    (120.00, 61, 0x04, '�������', '������', '������ � ����������'),
    (180.00, 90, 0x05, '��������', '������� ��������', '��������� �������'),
    (250.00, 91, 0x06, '�������', '����', '������ � �����'),
    (300.00, 150, 0x07, '�������', '������', '������ � ����������'),
    (90.00, 151, 0x08, '��������', '������� ��������', '������ � ��������'),
    (160.00, 22, 0x09, '�������', '����', '��������� �������'),
    (220.00, 17, 0x0A, '�������', '������', '������ � ����������');


SELECT * FROM SHEDULE_TABLE;

DELETE FROM SHEDULE_TABLE WHERE ID_SHEDULE BETWEEN 4 AND 12;

INSERT INTO SHEDULE_TABLE (MONDAY, TUESDAY, WEDNRSDAY, THURSDAY, FRIDAY, SATURDAY, SUNDAY)
VALUES 
   ('8:00 AM - 9:00 AM', '9:00 AM - 10:00 AM', '10:00 AM - 11:00 AM', '11:00 AM - 12:00 PM', '12:00 PM - 1:00 PM', '1:00 PM - 2:00 PM', '3:00 PM - 4:00 PM'),
  ('9:00 AM - 10:00 AM', '10:00 AM - 11:00 AM', '11:00 AM - 12:00 PM', '12:00 PM - 1:00 PM', '1:00 PM - 2:00 PM', '2:00 PM - 3:00 PM', '4:00 PM - 5:00 PM'),
  ('10:00 AM - 11:00 AM', '11:00 AM - 12:00 PM', '12:00 PM - 1:00 PM', '1:00 PM - 2:00 PM', '2:00 PM - 3:00 PM', '3:00 PM - 4:00 PM', '5:00 PM - 6:00 PM'),
  ('11:00 AM - 12:00 PM', '12:00 PM - 1:00 PM', '1:00 PM - 2:00 PM', '2:00 PM - 3:00 PM', '3:00 PM - 4:00 PM', '4:00 PM - 5:00 PM', '6:00 PM - 7:00 PM'),
  ('12:00 PM - 1:00 PM', '1:00 PM - 2:00 PM', '2:00 PM - 3:00 PM', '3:00 PM - 4:00 PM', '4:00 PM - 5:00 PM', '5:00 PM - 6:00 PM', '7:00 PM - 8:00 PM'),
  ('1:00 PM - 2:00 PM', '2:00 PM - 3:00 PM', '3:00 PM - 4:00 PM', '4:00 PM - 5:00 PM', '5:00 PM - 6:00 PM', '6:00 PM - 7:00 PM', '8:00 PM - 9:00 PM'),
  ('2:00 PM - 3:00 PM', '3:00 PM - 4:00 PM', '4:00 PM - 5:00 PM', '5:00 PM - 6:00 PM', '6:00 PM - 7:00 PM', '7:00 PM - 8:00 PM', '9:00 PM - 10:00 PM'),
  ('3:00 PM - 4:00 PM', '4:00 PM - 5:00 PM', '5:00 PM - 6:00 PM', '6:00 PM - 7:00 PM', '7:00 PM - 8:00 PM', '8:00 PM - 9:00 PM', '10:00 PM - 11:00 PM'),
  ('4:00 PM - 5:00 PM', '5:00 PM - 6:00 PM', '6:00 PM - 7:00 PM', '7:00 PM - 8:00 PM', '8:00 PM - 9:00 PM', '9:00 PM - 10:00 PM', '11:00 PM - 12:00 AM'),
  ('5:00 PM - 6:00 PM', '6:00 PM - 7:00 PM', '7:00 PM - 8:00 PM', '8:00 PM - 9:00 PM', '9:00 PM - 10:00 PM', '10:00 PM - 11:00 PM', '12:00 AM - 1:00 AM');

