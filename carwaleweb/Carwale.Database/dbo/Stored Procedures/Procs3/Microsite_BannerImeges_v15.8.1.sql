IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_BannerImeges_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_BannerImeges_v15]
GO

	-- =============================================
-- Author:		Sachin Shukla 
-- Create date: Aug,05 2015
-- Description:	This SP will be used for the home page Banner slider for Microsites.
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_BannerImeges_v15.8.1] 
	-- Add the parameters for the stored procedure here
	@DealerId INT
AS
BEGIN
	
	--SET NOCOUNT ON;
	begin try		
			SELECT Id,LandingURL, HostURL + DirectoryPath + LargeImage as URL,HostURL,DirectoryPath,LargeImage,BannerImgSortingOrder,OriginalImgPath
			FROM Microsite_Images 
			WHERE IsBanner = 1 AND DealerId = @DealerId AND IsActive = 1
			ORDER BY BannerImgSortingOrder
	end	try
	BEGIN catch
		 INSERT INTO Microsite_Exceptions(Programme_Name,TC_Exception,TC_Exception_Date)  
         VALUES('Microsite_BannerImages',ERROR_MESSAGE(),GETDATE())  
	END catch
END

