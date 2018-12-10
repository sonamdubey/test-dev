IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[ms].[GetDealerCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [ms].[GetDealerCars]
GO

	
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [ms].[GetDealerCars]
	-- Add the parameters for the stored procedure here
	@DealerId int,
	@makeid int,
	@modelid int = NULL,
	@versionid int = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	

    -- Insert statements for procedure here
		select TS.Id,vw.Car,TS.MakeYear,TS.Colour,TS.Kms,CP.ImageUrlThumbSmall,CF.FuelType,TS.Price,upper(TS.RegNo)RegNo
		,TS.EntryDate from TC_Stock as TS 
		left outer join TC_CarPhotos CP on CP.StockId=Ts.Id
		join vwMMV as vw on vw.VersionId=TS.VersionId		
		join CarFuelType CF on vw.CarFuelType=CF.FuelTypeId		
		where TS.BranchId=@DealerId
		and datediff(Year,TS.MakeYear,Getdate())>=2
		and vw.Makeid=@makeid 
		and CP.IsMain=1
		and (vw.ModelId = @modelid OR @modelid is NULL)
		and (vw.VersionId = @versionid OR @versionid is NULL)
END



