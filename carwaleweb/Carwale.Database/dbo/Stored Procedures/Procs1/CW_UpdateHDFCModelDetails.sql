IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_UpdateHDFCModelDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_UpdateHDFCModelDetails]
GO

	CREATE PROCEDURE [dbo].[CW_UpdateHDFCModelDetails]
@ModelIds varchar(5000)
,@IsActive bit,
@UpdatedBy INT = NULL,
@UpdatedOn DATETIME = NULL
AS 
--Author:Rakesh Yadav ON 02 Aug 2015
--Desc: Activate and deactivate CW_CarModelDetails
--Modifier : Vaibhav K added parameters for UpdatedBy & UpdatedOn

BEGIN
	UPDATE CW_CarModelDetails
	SET IsActive=@IsActive,
		UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn
	WHERE CarModelId IN (SELECT ListMember from fnSplitCSV(@ModelIds))
END