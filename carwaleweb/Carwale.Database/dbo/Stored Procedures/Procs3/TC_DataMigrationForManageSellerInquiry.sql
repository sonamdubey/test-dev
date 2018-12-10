IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DataMigrationForManageSellerInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DataMigrationForManageSellerInquiry]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 16-02-2012
-- Description:	Data migration for Manage Inquiry
-- Added for Testing remove once done
-- =============================================
CREATE PROCEDURE [dbo].[TC_DataMigrationForManageSellerInquiry]	
AS
BEGIN
	--set rowcount 0

DECLARE @rowCount  SMALLINT, 
        @loopCount SMALLINT=1,@Branchid  int
        
DECLARE @tc_userlooptable as table
(
  Id int identity(1,1) ,
  Branchid bigint
)

insert into @tc_userlooptable
Select distinct DPI.DealerId 
From AP_DealerPackageInquiries  AS DPI 
--where DPI.DealerId in (6,9)

SELECT @rowCount = COUNT(*) 
FROM   @tc_userlooptable 

WHILE @rowCount >= @loopCount -- loop through all inquiries 
  BEGIN 
  
       SELECT @Branchid = Branchid 
            FROM   @tc_userlooptable 
            WHERE  id = @loopCount 
            
      EXEC [dbo].[TEMPTC_AddSellerInquiryProcessSellersInquiries]  @Branchid     

      SET @loopCount=@loopCount + 1
      PRINT @loopCount  
  END 
END
