IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckLogShipping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckLogShipping]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 6-4-2014
-- Description:	Check Log Shipping
-- =============================================
CREATE PROCEDURE [dbo].[CheckLogShipping] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

     select * 
     from 
     (
		select top 3 Id,carversionid,requestdatetime,SourceId,ClientIP
		from NewCarPurchaseInquiries
		where SourceId=1
		order by id desc
	 ) as WPQ 
	
	union all
	
	select * 
     from 
     (
		select top 3 Id,carversionid,requestdatetime,SourceId,ClientIP
		from NewCarPurchaseInquiries
		where SourceId=43
		order by id desc
	 ) as MPQ 
	 
	union all
	
	select * 
     from 
     (
		select top 3 Id,carversionid,requestdatetime,SourceId,ClientIP
		from NewCarPurchaseInquiries
		where SourceId=74
		order by id desc
	 ) as APQ 
	
	


	--select top 3 Id,carversionid,requestdatetime,SourceId,ClientIP
	--from NewCarPurchaseInquiries
	--where SourceId=43
	--order by id desc

	--select top 3 Id,carversionid,requestdatetime,SourceId,ClientIP
	--from NewCarPurchaseInquiries
	--where SourceId=74
	--order by id desc
	
END
