IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DLS_CompetitionLead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DLS_CompetitionLead]
GO

	


CREATE PROCEDURE [dbo].[DLS_CompetitionLead]
	@CustomerId		NUMERIC,
	@VersionId		NUMERIC,
	@CityId			NUMERIC,
	@IsValid		BIT OUTPUT
 AS
	
BEGIN
	SET @IsValid = 0
	SELECT DL.Id FROM DLS_Leads AS DL WHERE DL.CWCustomerId = @CustomerId
	
	IF @@ROWCOUNT = 0
		BEGIN
			SELECT Id FROM DLS_Followups AS DF WHERE DF.CWCustomerId = @CustomerId
			IF @@ROWCOUNT = 0
				BEGIN
					SELECT ID FROM DLS_CompLeads WHERE CustomerId = @CustomerId
					IF @@ROWCOUNT = 0
						BEGIN
							SELECT Id FROM DLS_FiatCompList AS DF 
							WHERE DF.CarVersionId = @VersionId AND DF.CityId = @CityId
							
							IF @@ROWCOUNT <> 0
								BEGIN
									SET @IsValid = 1
								END
						END
				END
		END
END



