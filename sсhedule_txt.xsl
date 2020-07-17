<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
   xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <xsl:for-each select="schedule/week/day">
|<xsl:value-of select="@name_day"/>|<xsl:for-each select="lesson">
  |<xsl:value-of select="@title"/>|<xsl:value-of select="class"/>|<xsl:value-of select="teacher"/>|<xsl:value-of select="time/start"/>---<xsl:value-of select="time/end"/>|<xsl:value-of select="type"/>|</xsl:for-each>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>