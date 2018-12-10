IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDiscontinuedBikeModelsByMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDiscontinuedBikeModelsByMake]
GO

	-- =============================================
-- Author     :	Sangram Nandkhile 
-- Create date: 17 June 2016
-- Description:	procedure to get discontinued bikes for particular make
-- =============================================
CREATE PROCEDURE [dbo].[GetDiscontinuedBikeModelsByMake] 
	@MakeId int
AS
BEGIN
	--Select Make, Model, MakeMaskingName, modelmaskingname from vwMMV WITH(NOLOCK)
	--where IsModelNew= 0 AND MakeId=@MakeId
	Select MaskingName as modelmaskingName,name from bikemodels WITH(NOLOCK)
    where New = 0 and  BikeMakeId= @MakeId

END