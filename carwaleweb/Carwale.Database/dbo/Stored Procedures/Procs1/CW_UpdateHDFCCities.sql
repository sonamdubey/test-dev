IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_UpdateHDFCCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_UpdateHDFCCities]
GO

	-- =============================================
-- Author:		<Komal Manjare>
-- Create date: 27 august 2015
-- Description:	activate deactivate CW_CarCities
-- =============================================
CREATE PROCEDURE [dbo].[CW_UpdateHDFCCities] 
@SpokeCityIds VARCHAR(5000),
@IsActive BIT,
@UpdatedBy INT = NULL,
@UpdatedOn DATETIME = NULL	
AS
BEGIN
	UPDATE CW_CarCities
	SET IsActive=@IsActive,
		UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn
	WHERE SpokeCityId IN(SELECT ListMember FROM fnSplitCSV(@SpokeCityIds))
END

