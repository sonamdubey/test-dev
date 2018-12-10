IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiryExcelLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiryExcelLog]
GO

	-- Author		:	Tejashree
-- Create date	:	15/5/2013 
-- Description	:	This SP used to maintain record of uploaded exel sheet in the Trading Cars Software	this record initially will inactive       
-- =============================================    
CREATE PROCEDURE [dbo].[TC_InquiryExcelLog]    
 -- Add the parameters for the stored procedure here    
 @BranchId BIGINT,
 @UserId BIGINT,
 @InquiryType TINYINT,
 @InquirySourceId SMALLINT,
 @ExcelSheetName VARCHAR(50), 
 @FileName VARCHAR(100), 
 @DirPath VARCHAR(255),
 @HostUrl varchar(100),
 @Location varchar(255) OUTPUT,
 @ExcelSheetId BIGINT OUTPUT
    
AS    
BEGIN    
	 -- SET NOCOUNT ON added to prevent extra result sets from    
	 -- interfering with SELECT statements.    
	SET NOCOUNT ON;
	
	--SET @Location = 'http://'+@HostUrl+@DirPath+@BranchId+'_'+GETDATE()+'_'+@FileName
	SET @Location = 'http://'+@HostUrl+@DirPath+@FileName
	     
	--inserting record with inactive status,later once image will save in appropriate folder need to activate
	INSERT INTO TC_ExelSheetLog (DealerId,UserId,FileName,DirPath,HostUrl,InquiryType,EntryDate,StatusId,IsDeleted,Location,InquirySourceId,ExcelSheetName)    
	VALUES(@BranchId,@UserId,@FileName,@DirPath,@HostUrl,@InquiryType,GETDATE(),0,0,@Location,@InquirySourceId,@ExcelSheetName) 
	      
	SET @ExcelSheetId = SCOPE_IDENTITY();
	
END
