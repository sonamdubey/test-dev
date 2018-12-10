IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_UpdateNewCarLTV]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_UpdateNewCarLTV]
GO

	CREATE PROCEDURE [dbo].[CW_UpdateNewCarLTV]
@ModelIds varchar(50),
@IsActive bit,
@UpdatedBy INT = NULL,
@UpdatedOn DATETIME = NULL
AS
--Author:Rakesh Yadav On 1 Aug 2015
-- Desc: Activate and deactivate DW_NewCarLtv
BEGIN
	UPDATE CW_NewCarLTV
	SET IsActive=@IsActive,
		UpdatedBy = @UpdatedBy,
		UpdatedOn = @UpdatedOn
	WHERE CarModelId IN (SELECT ListMember from fnSplitCSV(@ModelIds))
END
