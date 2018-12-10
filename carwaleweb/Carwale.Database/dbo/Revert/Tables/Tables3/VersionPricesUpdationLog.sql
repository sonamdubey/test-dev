IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE NAME = 'VersionPricesUpdationLog'
			AND xtype = 'U'
		)
BEGIN
	DROP TABLE [VersionPricesUpdationLog]
END