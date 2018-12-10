IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DataMigrationForManageInquirySingleTCUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DataMigrationForManageInquirySingleTCUser]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 16-02-2012
-- Description:	Data migration for Manage Inquiry
-- =============================================
CREATE PROCEDURE [dbo].[TC_DataMigrationForManageInquirySingleTCUser]	
AS
BEGIN
	--set rowcount 0

DECLARE @rowCount  SMALLINT, 
        @loopCount SMALLINT=1 
        
DECLARE @tc_userlooptable as table
(
  Id int identity(1,1) ,
  Branchid bigint,
  UserId int
)

insert into @tc_userlooptable
select T.branchid,Id as UserId
from TC_Users as T WITH(NOLOCK)
join (
select branchid, COUNT(branchid) as cnt
from TC_Users WITH(NOLOCK)
where branchid not in (6,
9,
12,
21,
22,
34,
43,
46,
47,
50,
55,
59,
61,
65,
78,
83,
94,
102,
124,
129)
group by branchid
having count(branchid)=1
) a on a.BranchId=T.BranchId
order by T.branchid

SELECT @rowCount = COUNT(*) 
FROM   @tc_userlooptable 

WHILE @rowCount >= @loopCount -- loop through all inquiries 
  BEGIN 
      DECLARE @AssignedTo BIGINT, 
              @InqId      BIGINT, 
              @BranchId   BIGINT 

      SELECT @AssignedTo = userid, 
             @BranchId = branchid 
      FROM   @tc_userlooptable 
      WHERE  id = @loopCount 

    
      DECLARE @TblPurInq TABLE 
      ( 
        id     BIGINT IDENTITY(1, 1), 
        custid BIGINT
      ) 

      INSERT INTO @TblPurInq(custid) 
      SELECT customerid 
      FROM   tc_purchaseinquiries p WITH(NOLOCK)
      WHERE  p.branchid = @BranchId 
      ORDER  BY p.requestdatetime ASC 

      DECLARE @irowCount  SMALLINT, 
              @iloopCount SMALLINT=1 

      SELECT @irowCount = COUNT(*) 
      FROM   @TblPurInq 

      WHILE @irowCount >= @iloopCount -- loop through all inquiries 
        BEGIN 
            DECLARE @CustId BIGINT 

            SELECT @CustId = custid 
            FROM   @TblPurInq 
            WHERE  id = @iloopCount 

            EXEC [dbo].[Processbuyerinquiries] @CustId,@AssignedTo 

            SET @iloopCount=@iloopCount + 1 
        END 

      PRINT @loopCount 
      PRINT @iloopCount 

      SET @loopCount=@loopCount + 1 
  END 
END
