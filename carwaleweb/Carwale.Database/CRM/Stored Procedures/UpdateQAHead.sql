IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[UpdateQAHead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[UpdateQAHead]
GO

	CREATE PROCEDURE [CRM].[UpdateQAHead]

	@Id				INT,
	@HeadName		VARCHAR(250),
	@TotalWeight	FLOAT,
	@UpdatedBy		NUMERIC(18,0),
	@UpdateOn		DATETIME
	
	AS 
	BEGIN
		UPDATE CRM.QAHeads SET UpdatedOn = @UpdateOn ,HeadName=@HeadName, TotalWeight=@TotalWeight,
        UpdatedBy = @UpdatedBy WHERE Id = @Id AND IsActive=1
	END
