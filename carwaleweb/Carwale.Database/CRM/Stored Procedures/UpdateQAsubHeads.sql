IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[UpdateQAsubHeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[UpdateQAsubHeads]
GO

	CREATE PROCEDURE [CRM].[UpdateQAsubHeads]

	@Id				Int,
	@SubHeadName	Varchar(150),
	@TotalWeight	FLOAT,
	@UpdatedBy		numeric(18,0),
	@UpdateOn		Datetime,
	@Type			Varchar(10)


    AS
    BEGIN
		UPDATE CRM.QASubhead SET UpdatedOn = @UpdateOn ,SubheadName=@SubheadName, Weight=@TotalWeight,
        UpdateBy = @UpdatedBy,Type=@Type WHERE Id = @Id	AND IsActive=1
	END
	