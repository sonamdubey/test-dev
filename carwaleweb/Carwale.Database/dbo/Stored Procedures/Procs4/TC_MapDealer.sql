IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MapDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MapDealer]
GO

	-- =============================================
-- Author:		Tejashree Patil.
-- Create date: 10 Dec 2013.
-- Description:	This sp is used for Inventory(Stock) related dealer mapping.
-- EXEC TC_MapDealer 6703,3,'600,605,609,201'
-- EXEC TC_MapDealer null,3,NULL
-- =============================================
CREATE PROCEDURE	[dbo].[TC_MapDealer]
	@DealerAdminId INT,
	@CreatedBy INT,
	@MappingDealersList VARCHAR(1000)
AS
BEGIN
	
	DECLARE @TC_DealerMappingId INT = NULL

	--For Admin dealer
	IF NOT EXISTS(SELECT TOP 1 TC_DealerMappingId FROM TC_DealerMapping WHERE DealerAdminId = @DealerAdminId)
	BEGIN
		INSERT INTO TC_DealerMapping 	
					(DealerAdminId,EntryDate,CreatedBy)
		VALUES		(@DealerAdminId,GETDATE(),@CreatedBy)
				
		SET @TC_DealerMappingId=SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		DECLARE @IsActive BIT = 0
		SELECT	@TC_DealerMappingId = TC_DealerMappingId , @IsActive = IsActive
		FROM	TC_DealerMapping WITH(NOLOCK)
		WHERE	DealerAdminId = @DealerAdminId	

		IF(@IsActive = 0)
		BEGIN
			UPDATE	TC_DealerMapping
			SET		IsActive = 1
			WHERE	DealerAdminId = @DealerAdminId	
		END
	END
	
	--For Sub dealers
	DECLARE @OldValue VARCHAR(500) = NULL
	DECLARE @NewValue VARCHAR(500) = @MappingDealersList

	SELECT	@OldValue = COALESCE(@OldValue+',','') + CONVERT(VARCHAR,SD.SubDealerId) 
	FROM	TC_SubDealers SD WITH(NOLOCK)
	WHERE	TC_DealerMappingId = @TC_DealerMappingId 
			AND IsActive = 1

	--Updtae IsActive=0 for these records.
	/*
	UPDATE	TC_SubDealers SET IsActive=0, ModifiedDate = GETDATE()
	WHERE	SubDealerId IN (SELECT ListMember as DeletingData FROM fnSplitCSV(@OldValue)
							EXCEPT
							SELECT ListMember as DeletingData FROM fnSplitCSV(@NewValue)
							)
		*/
	--Insert these new records.
	INSERT INTO TC_SubDealers (TC_DealerMappingId,SubDealerId,EntryDate)
	SELECT	@TC_DealerMappingId, ListMember, GETDATE() 
	FROM	fnSplitCSV(@MappingDealersList)
	WHERE	ListMember NOT IN (	SELECT ListMember 
								FROM fnSplitCSV(@OldValue))

	EXEC TC_GetMappingDealersList

END

