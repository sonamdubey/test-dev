IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE NAME = 'InsertNewBlockedNumber'
			AND xtype = 'P'
		)
BEGIN
	DROP PROCEDURE InsertNewBlockedNumber
END
GO

-- =============================================
-- Author:		<Anuj Dhar>
-- Create date: <22/09/2016>
-- Description:	<Insert new blocked number>
-- Modified: Vicky Lund, 28-10-2016, Add IsAutoInserted column
-- =============================================
CREATE PROCEDURE [dbo].[InsertNewBlockedNumber] @Number VARCHAR(20)
	,@IsAutoInserted BIT = 0
AS
BEGIN
	IF NOT EXISTS (
			SELECT Mobile
			FROM BlockedNumbers WITH (NOLOCK)
			WHERE Mobile = @Number
			)
	BEGIN
		INSERT INTO BlockedNumbers (
			Mobile
			,IsAutoInserted
			)
		VALUES (
			@Number
			,@IsAutoInserted
			)
	END
END