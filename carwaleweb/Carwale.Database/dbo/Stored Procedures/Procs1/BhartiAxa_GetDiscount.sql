IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BhartiAxa_GetDiscount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BhartiAxa_GetDiscount]
GO

	-- =============================================
-- Author:		Ashish Verma
-- Create date: 23-06-2014
-- Description:	Show Discount of Bharti AXA when PQ is taken
-- Avishkar Modified 25-06-2014 to split Query
--Avishkar Approved 25-06-2014
-- =============================================
CREATE PROCEDURE [dbo].[BhartiAxa_GetDiscount]
	-- Add the parameters for the stored procedure here
	@CityId int,
	@ModelId int
	WITH RECOMPILE
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @BCity char(50),
	@BModel char(50)
	
	--Select TOP 1 BAID.Disount as Discount
	--From BhartiAxa_InsuranceDiscounts BAID with(nolock) inner join BhartiAxa_CarVersions BACV with(nolock) on BAID.ModelID = BACV.MODEL
	--inner join BhartiAxa_Carwale_MMV BACM with(nolock)  on BACM.RefrenceId=BACV.Reference_No
	--inner join BhartiAxa_Cities BAC with(nolock) on BAC.CW_City=BAID.City
	--Inner JOIN CarVersions Vs with(nolock) ON Vs.Id = BACM.CWVersionId
	--Inner JOIN CarModels CMO with(nolock)  ON CMO.ID= Vs.CarModelId
	--where BAC.CW_CityId=@CityId and CMO.ID=@ModelId

	select @BModel=BACV.MODEL
	from BhartiAxa_CarVersions BACV with(nolock)
	inner join BhartiAxa_Carwale_MMV BACM with(nolock)  on BACM.RefrenceId=BACV.Reference_No
	Inner JOIN CarVersions Vs with(nolock) ON Vs.Id = BACM.CWVersionId
	Inner JOIN CarModels CMO with(nolock)  ON CMO.ID= Vs.CarModelId
	where CMO.ID=@ModelId

	select  @BCity=BAC.CW_City
	from BhartiAxa_Cities BAC with(nolock) 
	where BAC.CW_CityId=@CityId

	Select TOP 1 BAID.Disount as Discount
	From BhartiAxa_InsuranceDiscounts BAID with(nolock) 	
	where City=@BCity and ModelID=@BModel

END

