IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DataMigrationForManageInquiryTCUserCarWale]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DataMigrationForManageInquiryTCUserCarWale]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 16-02-2012
-- Description:	Data migration for Manage Inquiry
-- =============================================
CREATE PROCEDURE [dbo].[TC_DataMigrationForManageInquiryTCUserCarWale]
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
group by branchid
having count(branchid)=2
) a on a.BranchId=T.BranchId
and UserName !='CarWale'
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
