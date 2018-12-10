IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RegisterSiteAffiliateUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RegisterSiteAffiliateUser]
GO

	
CREATE PROCEDURE [dbo].[RegisterSiteAffiliateUser]
	@Id			NUMERIC, -- 1 For Insertion & -1 for Updation
	@AffiliateMembersId	NUMERIC,	
	@SiteName		VARCHAR(100),	
	@SiteUrl		VARCHAR(100),	
	@SiteDescription	VARCHAR(1000),
	@SiteCategory		VARCHAR(100),
	@MembershipCode	VARCHAR(50)
	

AS
	BEGIN

		IF @Id = -1
			BEGIN

				SELECT Id FROM AffiliateMembersSites WHERE MembershipCode = @MembershipCode
				IF @@ROWCOUNT = 0 
					BEGIN
						INSERT INTO AffiliateMembersSites ( AffiliateMembersId, SiteName, SiteUrl, SiteDescription, 
						SiteCategory, MembershipCode )
	
						VALUES ( @AffiliateMembersId, @SiteName, @SiteUrl, @SiteDescription, 
						@SiteCategory, @MembershipCode )
					END
				ELSE
					BEGIN
						SET @MembershipCode = @MembershipCode + Convert(Varchar, @AffiliateMembersId)

						INSERT INTO AffiliateMembersSites ( AffiliateMembersId, SiteName, SiteUrl, SiteDescription, 
						SiteCategory, MembershipCode )
	
						VALUES ( @AffiliateMembersId, @SiteName, @SiteUrl, @SiteDescription, 
						@SiteCategory, @MembershipCode )
					END
	
			END
		ElSE
			BEGIN
				UPDATE AffiliateMembersSites 
				SET  SiteName = @SiteName, SiteUrl = @SiteUrl, SiteDescription = @SiteDescription, SiteCategory = @SiteCategory
				WHERE AffiliateMembersId = @AffiliateMembersId AND ID = @Id
			END	
	END
