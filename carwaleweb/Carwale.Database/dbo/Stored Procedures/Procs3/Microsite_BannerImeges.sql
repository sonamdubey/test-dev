IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_BannerImeges]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_BannerImeges]
GO

	-- =============================================
-- Author:		Chetan Kane
-- Create date: 06/03/2012
-- Description:	This SP will be used for the home page Banner slider for Microsites 
--Modified By :rakesh Yadav on 27 Jul 2015 to fetch HostURL,DirectoryPath,LargeImage,BannerImgSortingOrder
--Modified By: rakesh Yadav on 06 Aug 2015 to add originalImgPath
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_BannerImeges]  
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
