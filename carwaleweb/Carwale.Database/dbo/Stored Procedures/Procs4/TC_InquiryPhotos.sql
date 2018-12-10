IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiryPhotos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiryPhotos]
GO

	-- =============================================    
-- Author:  <Nilesh Utture>    
-- Create date: <26th October, 2012>    
-- Description: <Gives the CarName and returs status for Inquiries Images>    
/*DECLARE @Status TINYINT ,    
  @CarName VARCHAR(100)    
  EXEC TC_InquiryPhotos 9, 5, @Status OUTPUT, @CarName OUTPUT  
  SELECT @Status AS Status, @CarName AS CarName */  
-- Modified By : Tejashree Patil on 16 Jan 2013 at 5.30pm : Joined with TC_InquiriesLead instead of TC_Inquiries
-- =============================================    
CREATE  Procedure [dbo].[TC_InquiryPhotos]    
 -- Add the parameters for the stored procedure here    
@SellerInquiriesId BIGINT,    
@BranchId BIGINT,    
@Status TINYINT OUTPUT,    
@CarName VARCHAR(100)OUTPUT    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
 -- Modified By : Tejashree Patil on 16 Jan 2013
 --IF NOT EXISTS(SELECT TC_SellerInquiriesId FROM TC_SellerInquiries S INNER JOIN TC_Inquiries I ON S.TC_InquiriesId=I.TC_InquiriesId  WHERE TC_SellerInquiriesId=@SellerInquiriesId AND I.BranchId=@BranchId)    
 IF NOT EXISTS(	SELECT	TC_SellerInquiriesId 
				FROM	TC_SellerInquiries S 
						INNER JOIN	TC_InquiriesLead I -- Modified By : Tejashree Patil on 16 Jan 2013
									ON S.TC_InquiriesLeadId=I.TC_InquiriesLeadId  
				WHERE	TC_SellerInquiriesId=@SellerInquiriesId AND I.BranchId=@BranchId)    
  BEGIN    
   SET @Status=1    
  END    
 ELSE    
  BEGIN    
   -- Insert statements for procedure here    
       
	SELECT @CarName =( Ma.Name + ' ' + Mo.Name + ' ' + Ve.Name ) 
	FROM TC_SellerInquiries St WITH(NOLOCK)  
	INNER JOIN TC_InquiriesLead I WITH(NOLOCK)ON St.TC_InquiriesLeadId=I.TC_InquiriesLeadId  -- Modified By : Tejashree Patil on 16 Jan 2013
	INNER JOIN CarVersions Ve WITH(NOLOCK)ON Ve.Id = St.CarVersionId   
	INNER JOIN CarModels Mo WITH(NOLOCK)ON Mo.Id = Ve.CarModelId   
	INNER JOIN CarMakes Ma WITH(NOLOCK)ON Ma.Id = Mo.CarMakeId  
	WHERE St.TC_SellerInquiriesId = @SellerInquiriesId   
       
   SELECT Id, ImageUrlFull, ImageUrlThumb, ImageUrlThumbSmall, IsMain,DirectoryPath,HostUrl, StatusId FROM TC_SellCarPhotos WITH(NOLOCK)  
   WHERE IsActive = 1 AND TC_SellerInquiriesId = @SellerInquiriesId ORDER BY IsMain DESC, Id     
   SET @Status=0    
  END    
END
