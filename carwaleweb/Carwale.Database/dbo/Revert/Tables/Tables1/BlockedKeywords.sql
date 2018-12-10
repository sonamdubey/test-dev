IF EXISTS (
		SELECT *
		FROM sysobjects
		WHERE NAME = 'BlockedKeywords'
			AND xtype = 'U'
		)
BEGIN
	DROP TABLE [BlockedKeywords]
END