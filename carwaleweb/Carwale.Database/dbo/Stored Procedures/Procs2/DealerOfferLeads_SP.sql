IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DealerOfferLeads_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DealerOfferLeads_SP]
GO

	Create Procedure DealerOfferLeads_SP
	@OfferId	NUMERIC,
	@CustomerId	NUMERIC,
	@BuyTime	VARCHAR(50),
	@LeadId		NUMERIC OUTPUT
AS
	INSERT INTO DealerOfferLeads(OfferId, CustomerId, BuyTime, RequestDateTime) 
	Values(@OfferId, @CustomerId, @BuyTime, GetDate())

	SET @LeadId = Scope_Identity()