IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FetchOutletDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FetchOutletDetails]
GO
	
-- =============================================
-- Author:		Vivek GUpta 
-- Create date: 30-10-2014
-- Description:	To get Outlet details of dealers
-- =============================================
CREATE PROCEDURE [dbo].[TC_FetchOutletDetails]
@BranchId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	    
		DECLARE @ProfileImageUrl VARCHAR(300)

		SELECT @ProfileImageUrl = ((HostURL+DirectoryPath+ImgPathCustom300) + ',' + (HostURL+DirectoryPath+ImgPathCustom600)) FROM Microsite_Images WITH (NOLOCK)
		WHERE DealerId=@BranchId AND IsActive = 1 AND ImageCategoryId = 5  ORDER BY BannerImgSortingOrder asc
	    
		SELECT D.Organization SuperAdmin, D.StateId, D.CityId, D.AreaId, D.EmailId, D.MobileNo, D.PhoneNo, D.FaxNo, D.Address1,
			   D.Pincode, D.ContactHours, D.ContactPerson, D.MailerName, D.MailerEmailId,
               D.Status SuperAdminStatus, D.ExpiryDate SuperAdminExpiryDate, D.WebsiteUrl, D.FirstName,D.Longitude,D.Lattitude, @ProfileImageUrl AS ProfileImageUrl
        FROM Dealers D WITH (NOLOCK)
		WHERE  D.ID = @BranchId
END

