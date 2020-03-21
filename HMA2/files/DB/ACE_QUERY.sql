USE [ACE_TEST_LIVE]

---------------------------------------------------------------------------
-- USER 별 조회
---------------------------------------------------------------------------
select * from dbo.[USER] where UserId = 1263

-- ACE Manager
--update dbo.[USER] set [LEVEL] = 10, COMPANY_ID = 3835, COMPANY_NAME = 3835 where UserId = 1263

-- Elite Manager
--update dbo.[USER] set [LEVEL] = 8, COMPANY_ID = 1455, COMPANY_NAME = 1455 where UserId = 1263

-- Crown Manager
--update dbo.[USER] set [LEVEL] = 8, COMPANY_ID = 835, COMPANY_NAME = 835 where UserId = 1263
---------------------------------------------------------------------------

--select * from dbo.[ORDER] where PURCHASE_STR = '01-05139'
--update dbo.[ORDER]
--set [STATUS] = 6
--where ID = 87365


--select * from dbo.[ORDER] where PURCHASE_STR = '01-03717'
--update dbo.[ORDER]
--set [STATUS] = 6
--where ID = 36124


--select * from dbo.[USER] where UserId = 1263
--update dbo.[USER]
--set [LEVEL] = 8,
--    COMPANY_ID = 3835,
--	COMPANY_NAME = 3835
--where UserId = 1263


select * from dbo.[ORDER] where PURCHASE_STR = '01-05241'
select * from dbo.ITEM where [ORDER] = 87495
select * from dbo.RECEIVED_ITEMS where ITEM IN (1588106)

--update dbo.ITEM
--set [WEIGHT] = 50000
--where ID = 1588058

select * from dbo.PRODUCT where ID = 595

--(CASE WHEN P.MEASUREMENT = 'CASE' THEN (ISNULL(P.CASE_WT, 1) / (P.PACK_QTY * P.UNIT_QTY)) * I.ORDER_QTY
--	             WHEN P.MEASUREMENT = 'PACK' THEN (ISNULL(P.PACK_WT, 1) / (P.UNIT_QTY)) * I.ORDER_QTY
--	        ELSE 0
--			END )

--select CASE WHEN I.MEASUREMENT = 'CASE' THEN ISNULL(P.CASE_WT, 1) * I.ORDER_QTY
--            WHEN I.MEASUREMENT = 'PACK' THEN ISNULL(P.PACK_WT, 1) * I.ORDER_QTY
--	        ELSE (CASE WHEN ISNULL(P.CASE_WT, 0) > 0 THEN (ISNULL(P.CASE_WT, 1) / (P.PACK_QTY * P.UNIT_QTY)) * I.ORDER_QTY
--	                   ELSE (ISNULL(P.PACK_WT, 1) / (P.UNIT_QTY)) * I.ORDER_QTY
--			      END)
--	   END AS [WEIGHT]
--from dbo.ITEM AS I
--JOIN dbo.PRODUCT AS P ON I.PRODUCT = P.ID
--WHERE I.ID = 1588057


SELECT (ISNULL(P.CASE_WT, 1) * 1000) AS [WEIGHT] FROM dbo.PRODUCT AS P WHERE P.ID = 595