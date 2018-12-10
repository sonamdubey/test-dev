IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BhartiAxa_FillAutoCityandState]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BhartiAxa_FillAutoCityandState]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BhartiAxa_FillAutoCityandState] 
	-- Add the parameters for the stored procedure here
	@cityId float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT CW_CityId AS cityId,CW_StateId AS stateId from BhartiAxa_Cities where CW_CityId=@cityId
	SELECT CarwaleStateId AS stateId from [RTO Location] with(nolock) where CarWaleCityId=@cityId
END

