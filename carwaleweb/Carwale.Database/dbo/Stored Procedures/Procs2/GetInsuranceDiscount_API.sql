IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetInsuranceDiscount_API]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetInsuranceDiscount_API]
GO

	-- =============================================
-- Author:		<Author,,Ashish Verma>
-- Create date: <Create Date,06/10/2014,>
-- Description:	<Description,Returns both bhartiAxa and carwale dicount,>
-- Modified by Sourabh Roy on 29-09-2015 assign default value for output parameter
-- Modified by Sourabh Roy on 29-09-2015 added if block in case of null value of output parameters
-- =============================================
CREATE PROCEDURE [dbo].[GetInsuranceDiscount_API]
	-- Add the parameters for the stored procedure here
	@CityId int,
	@ModelId int,
	@CarwaleDiscount Int=0 Output,
	@BhartiAxaDiscount INT=0 Output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	declare @BCity char(50),
	@BModel char(50)

	select @BModel=BACV.MODEL
	from BhartiAxa_CarVersions BACV with(nolock)
	inner join BhartiAxa_Carwale_MMV BACM with(nolock)  on BACM.RefrenceId=BACV.Reference_No
	Inner JOIN CarVersions Vs with(nolock) ON Vs.Id = BACM.CWVersionId
	Inner JOIN CarModels CMO with(nolock)  ON CMO.ID= Vs.CarModelId
	where CMO.ID=@ModelId

	select  @BCity=BAC.CW_City
	from BhartiAxa_Cities BAC with(nolock) 
	where BAC.CW_CityId=@CityId

	Select TOP 1 @BhartiAxaDiscount = BAID.Disount
	From BhartiAxa_InsuranceDiscounts BAID with(nolock) 	
	where City=@BCity and ModelID=@BModel


	Select distinct @CarwaleDiscount = Discount
	From Con_InsuranceDiscount with(nolock)
	where CityId=@CityId and ModelId=@ModelId
	
	IF (@CarwaleDiscount IS NULL)
	BEGIN
	   SET @CarwaleDiscount=0;
	END 
	
	IF (@BhartiAxaDiscount IS NULL)
	BEGIN
	  
	  SET @BhartiAxaDiscount=0;
	 
	END 
	
	
END