IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_EditASC]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_EditASC]
GO

	

-- =============================================
-- Author:		Kumar Vikram
-- Create date: 20.12.2013
-- Description:	update user details
-- exec AxisBank_EditASC 16,'mumbaiq','mumbaiw',0
-- =============================================
CREATE PROCEDURE [dbo].[AxisBank_EditASC] @ID INT
	,@Name VARCHAR(50)
	,@NewName VARCHAR(50)
	,@Success BIT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ID
	FROM AxisBank_ASC
	WHERE ID = @ID

	IF @@ROWCOUNT > 0
	BEGIN
		SELECT ID
		FROM AxisBank_ASC
		WHERE NAME = @NewName

		IF @@ROWCOUNT > 0
			SET @Success = 0
		ELSE
		BEGIN
			UPDATE AxisBank_ASC
			SET NAME = @NewName
			WHERE ID = @ID

			SET @Success = 1
		END
	END
	ELSE
		SET @Success = 0

	select @Success
END


