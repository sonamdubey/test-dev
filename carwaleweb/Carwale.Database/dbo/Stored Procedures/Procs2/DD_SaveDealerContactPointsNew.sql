IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_SaveDealerContactPointsNew]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_SaveDealerContactPointsNew]
GO

	



-------------------------------------------------------------------------------------------


-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <4/11/2014>
-- Description:	<Save DealerOutlet and Contact Person's Contact Points>
--@ContactPts '1:02562-274666,2:8912121212,3:7878'
-- =============================================
CREATE PROCEDURE [dbo].[DD_SaveDealerContactPointsNew]
--@DD_DealerNamesId	INT,
@DD_DealerOutletId	INT,
@DD_ContactPerId	INT,
@CreatedBy			INT,
@ContactPts			VARCHAR(200),
@PrimaryNoCntType	INT,
@ContactNumber		VARCHAR(50),
@NewId				INT OUTPUT

AS
BEGIN
		SELECT VAL2,VAL1 FROM [dbo].[SplitTextbytwodelimiters](@ContactPts,',',':')
		EXCEPT
		SELECT ContactNumber,ContactType  FROM DD_ContactPoints --where DD_DealerNamesId = @DD_DealerNamesId

		IF @@ROWCOUNT = 0 
			SET @NewId = -1

		IF @DD_ContactPerId <> '0'
		BEGIN
			DELETE FROM DD_ContactPoints WHERE DD_ContactPersonId = @DD_ContactPerId

			INSERT INTO DD_ContactPoints (ContactNumber , ContactType , IsPrimary , DD_DealerOutletId ,DD_ContactPersonId, CreatedBy , CreatedOn)	
			SELECT VAL2,VAL1,0 , @DD_DealerOutletId, @DD_ContactPerId, @CreatedBy ,GETDATE() FROM [dbo].[SplitTextbytwodelimiters](@ContactPts,',',':')
			EXCEPT
			SELECT ContactNumber,ContactType ,0 , @DD_DealerOutletId , @DD_ContactPerId ,@CreatedBy ,GETDATE() FROM DD_ContactPoints --where DD_DealerNamesId = @DD_DealerNamesId
		
			SET @NewId = SCOPE_IDENTITY()


			SELECT ID FROM DD_ContactPoints WHERE ContactType = @PrimaryNoCntType AND DD_DealerOutletId = @DD_DealerOutletId AND DD_ContactPersonId = @DD_ContactPerId
			IF @@ROWCOUNT > 0
			BEGIN
				UPDATE DD_ContactPoints SET IsPrimary=0 WHERE DD_ContactPersonId = @DD_ContactPerId-- DD_DealerNamesId = @DD_DealerNamesId
				UPDATE DD_ContactPoints SET  IsPrimary=1 WHERE ContactType = @PrimaryNoCntType AND DD_DealerOutletId = @DD_DealerOutletId AND ContactNumber = @ContactNumber AND DD_ContactPersonId = @DD_ContactPerId
			END
		END
		ELSE IF @DD_DealerOutletId <> '0'
		BEGIN
			DELETE FROM DD_ContactPoints WHERE DD_DealerOutletId = @DD_DealerOutletId

			INSERT INTO DD_ContactPoints (ContactNumber , ContactType , IsPrimary  ,DD_DealerOutletId, CreatedBy , CreatedOn)	
			SELECT VAL2,VAL1,0 , @DD_DealerOutletId , @CreatedBy ,GETDATE() FROM [dbo].[SplitTextbytwodelimiters](@ContactPts,',',':')
			EXCEPT
			SELECT ContactNumber,ContactType ,0  , @DD_DealerOutletId ,@CreatedBy ,GETDATE() FROM DD_ContactPoints --where DD_DealerNamesId = @DD_DealerNamesId
		
			SET @NewId = SCOPE_IDENTITY()


			SELECT ID FROM DD_ContactPoints WHERE ContactType = @PrimaryNoCntType AND DD_DealerOutletId = @DD_DealerOutletId
			IF @@ROWCOUNT > 0
			BEGIN
				UPDATE DD_ContactPoints SET IsPrimary=0 WHERE DD_DealerOutletId = @DD_DealerOutletId-- DD_DealerNamesId = @DD_DealerNamesId
				UPDATE DD_ContactPoints SET  IsPrimary=1 WHERE ContactType = @PrimaryNoCntType AND ContactNumber = @ContactNumber AND DD_DealerOutletId = @DD_DealerOutletId
			END
		END
		 
END

