IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DLS_SaveLead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DLS_SaveLead]
GO

	
CREATE PROCEDURE [dbo].[DLS_SaveLead]
	@Id				NUMERIC,
	@Name			VARCHAR(250),
	@EMail			VARCHAR(150),
	@Mobile			VARCHAR(15),
	@Landline		VARCHAR(15),
	@CityId			NUMERIC,
	@CWCustomerId	NUMERIC,
	@LeadStage		SMALLINT,
	@ProductStatus	SMALLINT,
	@DealerId		NUMERIC,
	@UpdatedOn		DATETIME,
	@Comments		VARCHAR(50),
	@BuyTime		VARCHAR(15),
	@LeadType		SMALLINT,
	@CurrentId		NUMERIC OUTPUT
 AS
	
BEGIN
	SET @CurrentId = -1
	
	IF @Id = -1
		BEGIN
			INSERT INTO DLS_Leads
			(
				Name, EMail, Mobile, Landline, CityId, ProductStatus,
				CWCustomerId, LeadStage, DealerId, Comments, LeadType, BuyTime
			) 
			VALUES
			( 
				@Name, @EMail, @Mobile, @Landline, @CityId, @ProductStatus,
				@CWCustomerId, @LeadStage, @DealerId, @Comments, @LeadType, @BuyTime
			)
				
			SET @CurrentId = SCOPE_IDENTITY() 
		END
	ELSE
		BEGIN
			UPDATE DLS_Leads
			SET Name = @Name, EMail = @EMail, Mobile = @Mobile, 
				Landline = @Landline, CityId = @CityId, BuyTime = @BuyTime,
				Comments = @Comments, UpdatedOn = @UpdatedOn
			WHERE Id = @Id
			
			SET @CurrentId = @Id 
		END
END

