IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RemoveShowroomPrices]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RemoveShowroomPrices]
GO

	-- =============================================
-- Author:		<Raghupathy>
-- Create date: <10/4/2013>
-- Description:	<This Sp is used to remove prices for versions which are requested.>

-- Modified By : Sanjay Soni on 20/07/2016 added updatedby and lastupdated in input parameter, call sp to update NAP 
-- =============================================

CREATE PROCEDURE [dbo].[RemoveShowroomPrices]

	-- Add the parameters for the stored procedure here
	-- Parameters For NewCarShowroomPrices
	@CarVersions	INT,
	@CityId  	    INT,	-- CityId of that particular version
	@isMetallic     BIT,
	@UpdatedBy		INT,
	@LastUpdated	DATETIME
		-- Version Id's of all versions which are 

 AS

	BEGIN
	
		DELETE From CW_NewCarShowroomPrices WHERE CityId= @CityId AND CarVersionId = @CarVersions AND isMetallic=@isMetallic
		
		--Sync the NewCarShowroomPrices table
		EXEC [dbo].[UpdateNewCarPricesColor]  @VersionId=@CarVersions,
														  @CityId =@CityId,
														  @delColor= @isMetallic
		--update min and max price of the models in ModelMetroCityPrices table
		
		EXEC [dbo].[UpdateModelPrices]  @CarVersionId =@CarVersions,
	                                        @CityId =@CityId   

		EXEC [dbo].[Con_SaveNewCarNationalPrices] @CarVersions,@UpdatedBy,@LastUpdated
END





