IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_SaveConfigurableParameters]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_SaveConfigurableParameters]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 2nd June, 2015
-- Description:	To Save Configurable parameters for absure
-- Modified By : Suresh Prajapati on 12th June, 2015
-- Description : To Save Sequence type
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_SaveConfigurableParameters] @Id INT
	,@Category VARCHAR(MAX)
	,@Parameter VARCHAR(100)
	,@MinValue INT = NULL
	,@MaxValue INT = NULL
	,@ConstantValue INT = NULL
	,@EnteredBy INT = NULL
	,@UpdatedBy INT = NULL
	,@Status INT OUTPUT
	,@Sequence INT
AS
BEGIN
	IF @Id = - 1
	BEGIN
		SELECT * 
		FROM AbSure_ConfigurableParameters
		WHERE Category = @Category
			AND Parameter = @Parameter
			AND (MinValue = @MinValue OR MinValue IS NULL)
			AND (MaxValue = @MaxValue OR MaxValue IS NULL)
			AND (ConstantValue = @ConstantValue OR ConstantValue IS NULL)
			AND Sequence = @Sequence

			IF(@@ROWCOUNT <= 0)
			BEGIN
				INSERT INTO AbSure_ConfigurableParameters (
					Category
					,Parameter
					,MinValue
					,MaxValue
					,ConstantValue
					,EnteredBy
					,EntryDate
					,Sequence
					)
				VALUES (
					@Category
					,@Parameter
					,@MinValue
					,@MaxValue
					,@ConstantValue
					,@EnteredBy
					,GETDATE()
					,@Sequence
					)

				SET @Status = SCOPE_IDENTITY()
			END
	END
	ELSE
	BEGIN
		UPDATE AbSure_ConfigurableParameters
		SET Category = @Category
			,Parameter = @Parameter
			,MinValue = @MinValue
			,MaxValue = @MaxValue
			,ConstantValue = @ConstantValue
			,UpdatedBy = @UpdatedBy
			,UpdatedOn = GETDATE()
			,Sequence = @Sequence
		WHERE ID = @Id

		SET @Status = @Id
	END
END
