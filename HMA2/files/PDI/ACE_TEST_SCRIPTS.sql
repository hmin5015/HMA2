USE [ACE_TEST_LIVE]

-- PAYMENT 관련 기존 로직 -------------------------------------------------------------------------
--
-- PC: 
-- 무조건 ORDER 하나당 하나의 PAYMENT METHOD 를 입력하도록 되어있고,
-- PAYMENT 테이블에 저장하지 않는다. 
--
-- POS : 
-- ORDER 테이블에는 PAYMENT METHOD = NULL 또는 0 값을 입력하고,
-- PAYMENT 테이블에 멀티로 저장한다. 
--
-- 하지만, 이제 PAYMENT 로직은 변경되어야 한다. 
-- 
-- PC 에서도 ORDER 하나당 멀티의 PAYMENT 이 발생해야 하므로,
-- ORDER 테이블의 PAYMENT METHOD 에는 NULL 또는 0 값을 입력하도록 하고,
-- PAYMENT 테이블에 멀티로 저장하도록 한다. 
-- -----------------------------------------------------------------------------------------------

DECLARE @ID2 INT
SET @ID2 = 5358
SELECT ID, VENDOR, CUSTOMER, PAYMENT_METHOD, SUBTOTAL, TOTAL, ORDER_QTY, [STATUS], TOTAL_PAYMENTS, BALANCE_DUE, IS_ALLOCATE FROM dbo.[ORDER] WHERE ID = @ID2
select * from dbo.Payment where [ORDER] IN (SELECT ID FROM dbo.[ORDER] WHERE Is_POS = 0 AND ID = @ID2) order by Id Desc

--UPDATE dbo.[Payment] SET [PAYMENT_MEMO] = '2st Payment on 2019/09/06 by JHMIN' WHERE ID = 3589



-- USER 변경하기 ---------------------------------------------------------------------------------
-- 3835: ACE
-- 1455: Elite
-- 3835: CROWN
-------------------------------------------------------------------------------------------------
--SELECT * FROM dbo.[USER] WHERE UserId = 1263
--update dbo.[User] set [Level] = 8, COMPANY_ID = 835, COMPANY_NAME = 835 where UserId = 1263
--update dbo.[User] set [Level] = 10, COMPANY_ID = 3835, COMPANY_NAME = 3835 where UserId = 1263

DECLARE @PO_NO NVARCHAR(MAX) = '01-05096'
DECLARE @ORDERID INT
SELECT @ORDERID = ID FROM dbo.[ORDER] WHERE PURCHASE_STR = @PO_NO
SELECT * FROM dbo.[ORDER] WHERE ID = @ORDERID
SELECT * FROM dbo.ITEM WHERE [ORDER] = @ORDERID
SELECT * FROM dbo.RECEIVED_ITEMS WHERE ITEM IN (SELECT ID FROM dbo.ITEM WHERE [ORDER] = @ORDERID)
SELECT TOP 10 * FROM dbo.Item ORDER BY id DESC



-- Database Compatibility_Level 변경방법 --------------------------------------------------------
-- STRING_SPLIT 에러나는 경우 아래 구문 실행하면 된다. 
-- Ex: SELECT CONVERT(int,value) as id FROM STRING_SPLIT(@PURCHASE_STR, ',')
-------------------------------------------------------------------------------------------------
ALTER DATABASE [ACE_TEST_LIVE] SET COMPATIBILITY_LEVEL = 130