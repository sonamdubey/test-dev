IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsites_DeletBannerImage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsites_DeletBannerImage]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date: 2/03/2012-
-- Description:	Delete Banner Images uploaded by user for microsites home page
-- Modified By Vivek Gupta on 17-11-2014, removed carwale reference.
-- =============================================
CREATE PROCEDURE [dbo].[Microsites_DeletBannerImage] 
	-- Add the parameters for the stored procedure here
	(
		@PhotoId int,  @values varchar(100)
	)
AS
BEGIN

    Declare @BannerTable table
    (
         BannerImgSortingOrder smallint identity(0,1),
         id  bigint
         
         
    )
    
    update Microsite_Images set IsActive=0 where id =@PhotoId   

    -- Insert statements for procedure here
    
    insert into @BannerTable
    SELECT * FROM  [dbo].[fnSplitCSV] (@values)
    
    --select * from @BannerTable 
   
     
    -- -- updating for sorting order
     
     update Microsite_Images 
     set BannerImgSortingOrder=B.BannerImgSortingOrder
     from Microsite_Images as MI
        join @BannerTable as B on MI.Id=B.id     
	
END