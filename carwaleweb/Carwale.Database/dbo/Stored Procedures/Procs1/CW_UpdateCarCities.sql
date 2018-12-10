IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_UpdateCarCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_UpdateCarCities]
GO

	CREATE PROCEDURE [dbo].[CW_UpdateCarCities]
@CarCityIds varchar(200),
@IsActive bit,
@UpdatedBy INT = NULL,
@UpdatedOn DATETIME = NULL
AS
--Author:Vaibhav Kale On 5 Aug 2015
-- Desc: Activate and deactivate CW_CarCities
BEGIN
	UPDATE CW_CarCities
	SET IsActive=@IsActive,
		UpdatedBy = @UpdatedBy,
		UpdatedOn = @UpdatedOn
	WHERE Id IN (SELECT ListMember from fnSplitCSV(@CarCityIds))
END
